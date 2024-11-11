import { FC } from 'react';

import { ReactComponent as SpinnerIcon } from '../../assets/spinner.svg?react';
import SvgIcon from './SvgIcon';

const Loader: FC = () => {
  return (
    <div className="flex flex-grow items-center justify-center">
      <SvgIcon
        component={SpinnerIcon}
        className="custom-spinner max-md:w-16 max-md:h-16 w-40 h-40 text-gray-200 animate-spin dark:text-gray-600 fill-gray-600"
        alt="spinner"
      />
    </div>
  );
};

export default Loader;
