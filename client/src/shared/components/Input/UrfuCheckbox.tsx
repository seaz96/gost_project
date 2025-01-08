import type {ChangeEvent, InputHTMLAttributes} from "react";
import styles from './UrfuCheckbox.module.scss';

interface UrfuCheckboxProps extends InputHTMLAttributes<HTMLInputElement> {
    checked: boolean;
    title?: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const UrfuCheckbox = ({ checked, onChange, title, ...props }: UrfuCheckboxProps) => {
    return (
        <label className={styles.urfuCheckbox}>
            <input type="checkbox" checked={checked} onChange={onChange} {...props} />
            <span>{title}</span>
        </label>
    );
};

export default UrfuCheckbox;