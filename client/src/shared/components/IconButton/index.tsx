import type React from "react";

import classNames from "classnames";
import styles from "./IconButton.module.scss";

interface ButtonProps {
	children: React.ReactNode;
	isFilled?: boolean;
	className?: string;
	onClick: Function;
}

const IconButton: React.FC<ButtonProps> = (props) => {
	const { children, className, onClick, isFilled } = props;

	return (
		<button
			className={classNames(className, styles.baseButton, isFilled ? styles.filledButton : "")}
			onClick={(event) => onClick(event)}
		>
			{children}
		</button>
	);
};

export default IconButton;
