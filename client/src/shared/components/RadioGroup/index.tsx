import type React from "react";

import classNames from "classnames";
import styles from "./RadioGroup.module.scss";

interface RadioGroupProps {
	buttons: { id: string; value: string; label: string }[];
	name: string;
	value: string;
	onChange: Function;
	direction?: "vertical" | "horizontal";
}

const RadioGroup: React.FC<RadioGroupProps> = (props) => {
	const { buttons, name, value, onChange, direction = "horizontal" } = props;

	return (
		<div className={classNames(styles.buttonsGroup, direction === "vertical" ? styles.vertical : "")}>
			{buttons.map((button) => (
				<div key={button.id} className={styles.radioButton}>
					<input
						type="radio"
						id={button.id}
						name={name}
						value={button.value}
						checked={value === button.value ? true : false}
						onChange={() => onChange(button.value)}
					/>
					<label htmlFor={button.id}>{button.label}</label>
				</div>
			))}
		</div>
	);
};

export default RadioGroup;
