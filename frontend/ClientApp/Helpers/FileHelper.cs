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

        private async Task<File> GetFileTemplate(string route)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    File[] file = await response.Content.ReadAsAsync<File[]>();
                    return file[0];
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
            List<File> files = new List<File>();
            foreach(int id in group.files)
            {
                files.Add(await GetFileTemplate(String.Format("api/files/{0}", id)));
            }
            return files;
        }
    }
}
