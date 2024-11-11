export type TAuthResponse = {
  token: string;
  email: string;
  fullName: string;
  role: string;
};

export type TRegisterRequest = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
};

export type TLoginRequest = {
  email: string;
  password: string;
};

export type TApiResponse<T> = {
  message: string;
  data: T;
};
