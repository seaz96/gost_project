import { type ChangeEvent, type InputHTMLAttributes, forwardRef } from "react";
import styles from './UrfuCheckbox.module.scss';

interface UrfuCheckboxProps extends InputHTMLAttributes<HTMLInputElement> {
    checked: boolean;
    title?: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const UrfuCheckbox = forwardRef<HTMLInputElement, UrfuCheckboxProps>(({ checked, onChange, title, ...props }, ref) => {
    return (
        <label className={styles.urfuCheckbox}>
            <input type="checkbox" checked={checked} onChange={onChange} ref={ref} {...props} />
            <span>{title}</span>
        </label>
    );
});

export default UrfuCheckbox;