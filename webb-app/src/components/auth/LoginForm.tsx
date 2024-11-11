import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import type { TLoginRequest } from '../../types/userTypes';
import { Input } from '../common/Input';

export const LoginForm = () => {
  const { handleLogin } = useAuth();
  const {
    register,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useForm<TLoginRequest>();

  const onSubmit = async (data: TLoginRequest) => {
    try {
      await handleLogin(data);
    } catch {
      setError('root', { message: 'Invalid credentials' });
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <Input
        label="Email"
        type="email"
        {...register('email', { required: 'Email is required' })}
        error={errors.email?.message}
      />

      <Input
        label="Password"
        type="password"
        {...register('password', { required: 'Password is required' })}
        error={errors.password?.message}
      />

      {errors.root && <p className="text-sm text-red-500 text-center">{errors.root.message}</p>}

      <button
        type="submit"
        disabled={isSubmitting}
        className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600 disabled:opacity-50"
      >
        {isSubmitting ? 'Loading...' : 'Login'}
      </button>

      <Link to="/register" className="block text-sm text-center text-blue-500">
        Need an account? Register
      </Link>
    </form>
  );
};
