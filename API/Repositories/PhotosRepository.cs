using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Dapper;

namespace API.Repositories
{
    public interface IPhotosRepository : IGenericRepositoryBase<Photo,VPhoto>
    {
        public Task<int> Update(IDbConnection connection, VPhoto photo);
        public Task<bool> HasUserAnyPhotos(IDbConnection connection, int userId);
        public Task<int> SetMainPhoto(IDbConnection connection, int photoId);
        public Task<VPhoto> Add(IDbConnection connection, Photo photo);
    }
    public class PhotosRepository : GenericRepositoryBase<Photo, VPhoto>, IPhotosRepository
    {
        public async Task<VPhoto> Add(IDbConnection connection, Photo photo)
        {
            if(connection.State == ConnectionState.Open)
            {
                string sQuery = @"Insert Into Photo (Url, IsMain, PublicId, AppUserId) values (@Url, @IsMain, @PublicId, @AppUserId) RETURNING *;";
                var result = await connection.QuerySingleAsync<VPhoto>(sQuery,photo);
                return result;
            }
            return null;
        }

        public async Task<VPhoto> GetMainPhotoByUserId(IDbConnection connection, int userId)
        {
            if(connection.State == ConnectionState.Open)
            {
                string sQuery = @"Select * FROM Photo Where appUserId=@AppUserId AND isMain=@IsMain;";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("AppUserId",userId);
                dynamicParameters.Add("IsMain",true);
                var result = await connection.QueryFirstOrDefaultAsync<VPhoto>(sQuery,new {AppUserId = userId, IsMain=true});
                return result;
            }
            return null;
        }

        public async Task<bool> HasUserAnyPhotos(IDbConnection connection, int userId)
        {
            if(connection.State == ConnectionState.Open)
            {
                string sQuery = @"Select Id FROM Photo Where appUserId=@AppUserId Limit 1;";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("AppUserId",userId);
                var result = await connection.QueryFirstOrDefaultAsync<VPhoto>(sQuery,new {AppUserId = userId});
                return true;
            }
            return false;
        }

        public async Task<int> SetMainPhoto(IDbConnection connection, int photoId)
        {
            if(connection.State == ConnectionState.Open)
            {
                  string sQuery = @"
UPDATE photo 
SET
IsMain = @IsMain,
WHERE Id = @Id;";

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("IsMain", true);
                dynamicParameters.Add("Id", photoId);

                return await connection.ExecuteAsync(sQuery, photoId);
            }
            return 0;
        }

        public async Task<int> Update(IDbConnection connection, VPhoto photo)
        {
            if(connection.State == ConnectionState.Open)
            {
                string sQuery = @"
UPDATE photo 
SET
Url = @Url,
IsMain = @IsMain,
PublicId = @PublicId,
AppUserId = @AppUserId
WHERE Id = @Id;";

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Url", photo.Url);
                dynamicParameters.Add("IsMain", photo.IsMain);
                dynamicParameters.Add("PublicId", photo.PublicId);
                dynamicParameters.Add("AppUserId", photo.AppUserId);
                dynamicParameters.Add("Id", photo.Id);

                return await connection.ExecuteAsync(sQuery, photo);
            }

            return 0;
        }
    }
}