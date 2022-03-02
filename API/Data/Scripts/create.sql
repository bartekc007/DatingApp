Create Database DatingAppData;

Create table if NOT Exists appUser (
    appUserId BIGSERIAL PRIMARY KEY ,
    userName varchar(40) NOT NULL ,
    passwordHash bytea NOT NULL ,
    passwordSalt bytea NOT NULL ,
    dateOfBirth DATE NOT NULL ,
    knownAs varchar NOT NULL ,
    created TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
    lastActive TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
    gender varchar ,
    introduction varchar(255) ,
    lookingFor varchar ,
    interests varchar ,
    city varchar ,
    country varchar 
);

Create table if not exists photo(
    photoId BIGSERIAL PRIMARY KEY ,
    url varchar ,
    isMain boolean DEFAULT FALSE ,
    publicId varchar ,
    appUserId int ,

    CONSTRAINT fk_appUser
        FOREIGN KEY(appUserId)
            REFERENCES appUser(appUserId)
);

Create or Replace View vMember
AS
SELECT U.appUserId as Id, 
       U.username, 
       U.dateOfBirth, 
       U.knownAs, 
       U.created, 
       U.lastActive,
       U.gender, 
       U.introduction, 
       U.lookingFor, 
       U.interests, 
       U.city, 
       U.country, 
       P.url as photoUrl
FROM  appUser U
INNER JOIN
    photo P on U.appUserId = P.appUserId
where P.isMain = true;

Create or Replace View vPhoto
AS 
Select photoId AS Id,
       url,
       isMain,
       appUserId
From photo;


