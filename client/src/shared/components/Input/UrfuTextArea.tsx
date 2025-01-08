import { forwardRef, useId } from 'react';
import styles from './UrfuTextArea.module.scss';

interface UrfuTextAreaProps extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {
    label?: string;
    error?: string;
}

const UrfuTextArea = forwardRef<HTMLTextAreaElement, UrfuTextAreaProps>(({ label, error, id, ...props }, ref) => {
    const generatedId = useId();
    const textAreaId = id || generatedId;

    return (
        <div className={styles.textAreaContainer}>
            {label && <label htmlFor={textAreaId} className={styles.label}>{label}</label>}
            <textarea
                ref={ref}
                id={textAreaId}
                className={`${styles.textArea} ${error ? styles.error : ''}`}
                {...props}
            />
            {error && <span className={styles.errorMessage}>{error}</span>}
        </div>
    );
});

export default UrfuTextArea;