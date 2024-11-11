import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import type { TRegisterRequest } from '../../types/userTypes';
import { Input } from '../common/Input';

type TRegisterForm = TRegisterRequest & { confirmPassword: string };

export const RegisterForm = () => {
  const { handleRegister } = useAuth();
  const {
    register,
    handleSubmit,
    setError,
    watch,
    formState: { errors, isSubmitting },
  } = useForm<TRegisterForm>();

  const onSubmit = async (data: TRegisterForm) => {
    try {
      const { confirmPassword, ...registerData } = data;
      await handleRegister(registerData);
    } catch {
      setError('root', { message: 'Registration failed' });
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <Input
        label="First Name"
        {...register('firstName', { required: 'First name is required' })}
        error={errors.firstName?.message}
      />

      <Input
        label="Last Name"
        {...register('lastName', { required: 'Last name is required' })}
        error={errors.lastName?.message}
      />

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

      <Input
        label="Confirm Password"
        type="password"
        {...register('confirmPassword', {
          required: 'Please confirm password',
          validate: (val) => val === watch('password') || 'Passwords do not match',
        })}
        error={errors.confirmPassword?.message}
      />

      {errors.root && <p className="text-sm text-red-500 text-center">{errors.root.message}</p>}

      <button
        type="submit"
        disabled={isSubmitting}
        className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600 disabled:opacity-50"
      >
        {isSubmitting ? 'Loading...' : 'Register'}
      </button>

      <Link to="/login" className="block text-sm text-center text-blue-500">
        Already have an account? Login
      </Link>
    </form>
  );
};
