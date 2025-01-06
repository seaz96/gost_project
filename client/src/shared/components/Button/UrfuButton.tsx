import classNames from "classnames";
import styles from "./UrfuButton.module.scss";

interface UrfuButtonProps {
    children: React.ReactNode;
    color?: "red" | "blue";
    size?: "medium" | "large";
    type?: "button" | "submit" | "reset";
    outline?: boolean;
    onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

const UrfuButton: React.FC<UrfuButtonProps> = ({ children, color = "blue", size = "medium", outline = false, onClick, type = "button" }) => {
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        if (onClick) {
            onClick(event);
        }
    };

    return (
        <button
            type={type}
            className={classNames(styles.urfuButton, styles[color], styles[size], { [styles.outline]: outline })}
            onClick={handleClick}
        >
            {children}
        </button>
    );
};

export default UrfuButton;