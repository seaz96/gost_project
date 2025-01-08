import { forwardRef } from 'react';
import styles from './UrfuTextInput.module.scss';

interface UrfuTextInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    error?: string;
}

const UrfuTextInput = forwardRef<HTMLInputElement, UrfuTextInputProps>(({ label, error, ...props }, ref) => {
    return (
        <div className={styles.inputContainer}>
            {label && <label htmlFor={props.id} className={styles.label}>{label}</label>}
            <input
                ref={ref}
                className={`${styles.input} ${error ? styles.error : ''}`}
                {...props}
            />
            {error && <span className={styles.errorMessage}>{error}</span>}
        </div>
    );
});

export default UrfuTextInput;