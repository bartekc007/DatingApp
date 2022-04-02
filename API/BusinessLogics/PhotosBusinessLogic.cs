using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Repositories;

namespace API.BusinessLogics
{
    public interface IPhotosBusinessLogic : IGenericBusinessLogicBase<Photo,VPhoto>
    {
        public Task<int> Update(VPhoto photo);
        public Task<bool> HasUserAnyPhotos(int userId);
        public Task<int> SetMainPhoto(int photoId);
        public Task<VPhoto> Add(Photo photo);
    }
    public class PhotosBusinessLogic : GenericBusinessLogicBase<Photo, VPhoto, IPhotosRepository>, IPhotosBusinessLogic
    {
        public PhotosBusinessLogic(Func<DbConnectionFactory> factory, IPhotosRepository repository) : base(factory, repository) {}

        public async Task<VPhoto> Add(Photo photo)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                var response = await _repository.Add(connection, photo);
                return response;
            }
        }

        public async Task<bool> HasUserAnyPhotos(int userId)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                var response = await _repository.HasUserAnyPhotos(connection, userId);
                return response;
            }
        }

        public async Task<int> SetMainPhoto(int photoId)
        {
            using(IDbConnection connection = _context().Connection){
                connection.Open();
                var result = await _repository.SetMainPhoto(connection, photoId);
                return result;
            }
        }

        public async Task<int> Update(VPhoto photo)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                var response = await _repository.Update(connection,photo);
                return response;
            }
        }
    }
}