using API_MortalKombat.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceGeneric<T,A> where T : class
    {
        public Task<APIResponse> GetAll();
        public Task<APIResponse> GetById(int id);
        public Task<APIResponse> GetByName(string name);
        public Task<APIResponse> Create([FromBody] A armaCreateDto);
        public Task<APIResponse> Update([FromBody] T armaUpdateDto);
        public Task<APIResponse> Delete(int id);
        public Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<T> armaUpdateDto);
    }
}
