import { RegisterForm } from '../components/auth/RegisterForm';
const RegisterPage = () => {
  return (
    <div className="min-h-screen bg-gray-100 flex flex-col justify-center">
      <div className="max-w-md w-full mx-auto">
        <div className="bg-white p-8 border border-gray-300 rounded-lg shadow-lg">
          <h2 className="text-2xl font-bold text-center text-gray-800 mb-8">Create an Account</h2>
          <RegisterForm />
        </div>
      </div>
    </div>
  );
};

export default RegisterPage;
