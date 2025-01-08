import classNames from "classnames";
import styles from "./Input.module.scss";

export type CustomInputType = "email" | "money" | "number" | "password" | "phone" | "text" | "zip" | "date";

interface InputProps {
	label?: string;
	placeholder?: string;
	required?: boolean;
	type: CustomInputType;
	className?: string;
	onChange?: Function;
	onBlur?: Function;
	value?: string | number | undefined;
	onKeyUp?: Function;
	id?: string;
	name?: string;
	error?: string;
}

const Input: React.FC<InputProps> = (props) => {
	const {
		label,
		placeholder = label,
		required,
		type,
		className,
		onChange = () => {},
		onBlur = () => {},
		onKeyUp = () => {},
		value,
		id,
		name,
		error,
	} = props;

	const renderRequiredLabel = (): JSX.Element => <span className="input-required">*</span>;

	const inputID: string = id || label?.toLowerCase() || "";

	return (
		<div className={classNames(styles.inputContainer, className)}>
			{label ? (
				<label htmlFor={inputID} className={styles.baseLabel}>
					{label} {required ? renderRequiredLabel() : null}
				</label>
			) : (
				<></>
			)}
			<input
				id={inputID}
				type={type}
				name={name || inputID}
				placeholder={placeholder}
				onChange={(event) => onChange(event.target.value)}
				onBlur={(event) => onBlur(event.target.value)}
				required={required ?? false}
				value={value}
				className={styles.baseInput}
				onKeyUp={(event) => onKeyUp(event)}
			/>
			{error && <p className={styles.error}>{error}</p>}
		</div>
	);
};

export default Input;
