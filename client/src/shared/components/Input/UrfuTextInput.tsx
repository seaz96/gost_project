import { forwardRef, useId } from "react";
import styles from "./UrfuTextInput.module.scss";

interface UrfuTextInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
	label?: string;
	error?: string;
}

const UrfuTextInput = forwardRef<HTMLInputElement, UrfuTextInputProps>(({ label, error, id, ...props }, ref) => {
	const generatedId = useId();
	//FIXME
	const inputId = id || generatedId;

	return (
		<div className={styles.inputContainer}>
			{label && (
				<label htmlFor={inputId} className={styles.label}>
					{label}
				</label>
			)}
			<input ref={ref} id={inputId} className={`${styles.input} ${error ? styles.error : ""}`} {...props} />
			{error && <span className={styles.errorMessage}>{error}</span>}
		</div>
	);
});

export default UrfuTextInput;
