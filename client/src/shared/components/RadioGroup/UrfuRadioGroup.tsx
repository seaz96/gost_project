import { forwardRef } from "react";
import styles from "./UrfuRadioGroup.module.scss";

interface RadioOption {
	value: string;
	label: string;
}

interface UrfuRadioGroupProps {
	options: RadioOption[];
	label?: string;
	error?: string;
	value?: string;
	name: string;
	multiline?: boolean;
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

const UrfuRadioGroup = forwardRef<HTMLInputElement, UrfuRadioGroupProps>(
	({ options, label, error, value, name, onChange, ...props }, ref) => {
		return (
			<div className={styles.radioGroupContainer}>
				{label && <div className={styles.label}>{label}</div>}
				<div className={styles.radioGroup}>
					{options.map((option) => (
						<label key={option.value} className={styles.radioLabel}>
							<input
								type="radio"
								value={option.value}
								checked={value === option.value}
								name={name}
								onChange={onChange}
								ref={ref}
								{...props}
							/>
							<span className={styles.customRadio} />
							<span className={styles.radioText}>{option.label}</span>
						</label>
					))}
				</div>
				{error && <span className={styles.error}>{error}</span>}
			</div>
		);
	},
);

export default UrfuRadioGroup;
