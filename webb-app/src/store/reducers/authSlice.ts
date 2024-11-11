import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { TAuthResponse, TUser } from '../../types/userTypes';

type TAuthState = {
  user: TUser | null;
  token: string | null;
  isAuthenticated: boolean;
};

const initialState: TAuthState = {
  user: null,
  token: localStorage.getItem('token'),
  isAuthenticated: !!localStorage.getItem('token'),
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<TAuthResponse>) => {
      const { token, ...userData } = action.payload;
      state.user = userData;
      state.token = token;
      state.isAuthenticated = true;
      localStorage.setItem('token', token);
    },
    logout: (state) => {
      state.user = null;
      state.token = null;
      state.isAuthenticated = false;
      localStorage.removeItem('token');
    },
  },
});

export const { setCredentials, logout } = authSlice.actions;
export default authSlice.reducer;
