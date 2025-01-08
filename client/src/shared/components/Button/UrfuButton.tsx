import classNames from "classnames";
import styles from "./UrfuButton.module.scss";

interface UrfuButtonProps {
	children: React.ReactNode;
	color?: "red" | "blue";
	size?: "medium" | "large" | "small";
	type?: "button" | "submit" | "reset";
	outline?: boolean;
	disabled?: boolean;
	onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

const UrfuButton = ({
	children,
	color = "blue",
	size = "medium",
	outline = false,
	onClick,
	disabled = false,
	type = "button",
}: UrfuButtonProps) => {
	const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
		if (onClick) {
			onClick(event);
		}
	};

	return (
		<button
			type={type}
			disabled={disabled}
			className={classNames(styles.urfuButton, styles[color], styles[size], { [styles.outline]: outline })}
			onClick={handleClick}
		>
			{children}
		</button>
	);
};

export default UrfuButton;
