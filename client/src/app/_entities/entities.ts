export interface User {
    username: string;
    id: number;
    token: string;
}

export interface MemberDto {
    id: number;
    username: string;
    photoUrl: string;
    dateOfBirth: Date;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: PhotoDto[];
}

export interface LoginDto {
    userName: string;
    password: string;
}

export interface RegisterDto {
    userName: string;
    password: string;
    dateOfBirth: Date;
    knownAs: string;
}

export interface PhotoDto {
    id: number;
    url: string;
    isMain: boolean;
}