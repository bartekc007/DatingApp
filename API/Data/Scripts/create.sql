

Create table users (
    id int PRIMARY KEY,
    user_name varchar(40) NOT NULL,
    passwordhash bytea NOT NULL,
    passwordsalt bytea NOT NULL,
)