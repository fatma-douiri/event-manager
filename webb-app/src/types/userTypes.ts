export type TUser = {
  email: string;
  fullName: string;
  role: string;
};

export type TAuthResponse = TUser & {
  token: string;
};

export type TLoginRequest = {
  email: string;
  password: string;
};

export type TRegisterRequest = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
};

export type TRegisterFormInputs = TRegisterRequest & {
  confirmPassword: string;
};

// Enums
export const EUserRole = {
  Organizer: 'Organizer',
  Participant: 'Participant',
} as const;

export type TUserRole = (typeof EUserRole)[keyof typeof EUserRole];
