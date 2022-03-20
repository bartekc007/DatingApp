Create Database DatingAppData;

Create table if NOT Exists AppUser (
    Id BIGSERIAL PRIMARY KEY ,
    Username varchar(40) NOT NULL ,
    PasswordHash bytea NOT NULL ,
    PasswordSalt bytea NOT NULL ,
    DateOfBirth DATE NOT NULL ,
    KnownAs varchar NOT NULL ,
    Created TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
    LastActive TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
    Gender varchar ,
    Introduction varchar(255) ,
    LookingFor varchar ,
    Interests varchar ,
    City varchar ,
    Country varchar 
);

Create table if not exists Photo(
    Id BIGSERIAL PRIMARY KEY ,
    Url varchar ,
    IsMain boolean DEFAULT FALSE ,
    PublicId varchar ,
    AppUserId int ,

    CONSTRAINT fk_appUser
        FOREIGN KEY(appUserId)
            REFERENCES appUser(appUserId)
);

Create or Replace View vMember
AS
SELECT U.Id, 
       U.Username, 
       U.DateOfBirth, 
       U.KnownAs, 
       U.Created, 
       U.LastActive,
       U.Gender, 
       U.Introduction, 
       U.LookingFor, 
       U.Interests, 
       U.City, 
       U.Country, 
       P.Url as PhotoUrl
FROM  AppUser U
INNER JOIN
    Photo P on U.Id = P.Id
where P.IsMain = true;

Create or Replace View vPhoto
AS 
Select Id,
       Url,
       IsMain,
       AppUserId
From Photo;


