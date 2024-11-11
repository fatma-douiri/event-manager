import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

type TPrivateRouteProps = {
  children: React.ReactNode;
  roles?: string[];
};

export const PrivateRoute = ({ children, roles }: TPrivateRouteProps) => {
  const { isAuthenticated, user } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (roles && user && !roles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
};
