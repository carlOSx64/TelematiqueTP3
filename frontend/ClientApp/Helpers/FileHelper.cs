using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace ClientApp.Helpers
{
    class FileHelper
    {
        HttpClient httpc;

        public FileHelper(HttpClient httpc)
        {
            this.httpc = httpc;
        }

        private async Task<List<File>> GetFileTemplate(string route)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    File[] file = await response.Content.ReadAsAsync<File[]>();
                    return new List<File>(file);
                }

                throw new Exception();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<File>> GetGroupFiles(Group group)
        {
            return await GetFileTemplate(String.Format("api/group/{0}/files", group.Id));
        }
    }
}
