import { FC, SVGProps } from 'react';

interface SvgIconProps {
  alt: string;
  component: FC<SVGProps<SVGSVGElement>>;
  className?: string;
  color?: string;
  size?: number;
}

const SvgIcon: FC<SvgIconProps> = ({
  component: SVGComponent,
  alt,
  size = 24,
  color = 'currentColor',
  className,
  ...props
}) => (
  <SVGComponent
    width={size}
    height={size}
    fill={color}
    className={className}
    aria-label={alt}
    {...props}
  />
);

export default SvgIcon;
