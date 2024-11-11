import { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { logout, setCredentials } from '../store/reducers/authSlice';
import { useLoginMutation, useRegisterMutation } from '../store/services/authService';
import type { RootState } from '../store/store';
import type { TLoginRequest, TRegisterRequest } from '../types/userTypes';

export const useAuth = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [login, { isLoading: isLoginLoading }] = useLoginMutation();
  const [register, { isLoading: isRegisterLoading }] = useRegisterMutation();

  const { user, isAuthenticated } = useSelector((state: RootState) => state.auth);

  const handleLogin = useCallback(
    async (credentials: TLoginRequest) => {
      try {
        const response = await login(credentials).unwrap();
        dispatch(setCredentials(response));
        navigate('/events');
        return response;
      } catch (error) {
        console.error('Login failed:', error);
        throw error;
      }
    },
    [login, dispatch, navigate],
  );

  const handleRegister = useCallback(
    async (userData: TRegisterRequest) => {
      try {
        const response = await register(userData).unwrap();
        dispatch(setCredentials(response));
        navigate('/events');
        return response;
      } catch (error) {
        console.error('Registration failed:', error);
        throw error;
      }
    },
    [register, dispatch, navigate],
  );

  const handleLogout = useCallback(() => {
    dispatch(logout());
    navigate('/login');
  }, [dispatch, navigate]);

  return {
    user,
    isAuthenticated,
    handleLogin,
    handleRegister,
    handleLogout,
    isLoginLoading,
    isRegisterLoading,
  };
};
