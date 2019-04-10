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

        public async Task<File> GetFileTemplate(string route)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<File>();
                }

                throw new Exception();
            }
            catch(Exception e)
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

        public async Task<bool> CreateFile(string name, byte[] fileContent, int groupId)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                
                var fileData = new Dictionary<string, string> { { "name", name }, { "content", System.Convert.ToBase64String(fileContent) }, { "groupId", groupId.ToString() } };
                HttpContent content = new FormUrlEncodedContent(fileData);

                try
                {
                    HttpResponseMessage response = await httpc.PostAsync("api/files/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    throw new Exception();
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteFile(int fileId)
        {
                try
                {
                    HttpResponseMessage response = await httpc.DeleteAsync("api/files/"+fileId.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    throw new Exception();
                }
                catch
                {
                    return false;
                }
        }
    }
}
