import { Add, Remove } from "@mui/icons-material";
import { useState } from "react";
import styles from "./FilterButton.module.scss";

interface FilterButtonProps {
	title: string;
	options: Option[];
	selectedOptions: string[];
	setSelectedOptions: (options: string[]) => void;
}

interface Option {
	value: string;
	label: string;
}

const FilterButton = ({ options, selectedOptions, setSelectedOptions, title }: FilterButtonProps) => {
	const [isOpen, setIsOpen] = useState(false);

	const toggleDropdown = () => setIsOpen(!isOpen);

	const handleOptionClick = (option: string) => {
		if (selectedOptions.includes(option)) {
			setSelectedOptions(selectedOptions.filter((item) => item !== option));
		} else {
			setSelectedOptions([option, ...selectedOptions]);
		}
	};

	return (
		<div className={`${styles.filterButton} ${selectedOptions.length > 0 ? styles.selected : ""}`}>
			<button type={"button"} onClick={toggleDropdown}>
				{selectedOptions.length > 0 && <div className={styles.selectedCount}>{selectedOptions.length}</div>}
				<span>{title}</span>
				<span className={styles.openIcon}>{isOpen ? <Remove /> : <Add />}</span>
			</button>
			{isOpen && (
				<div className={styles.dropdown}>
					{options.map((option) => (
						<div
							onKeyDown={(e) => e.key === "Enter" && handleOptionClick(option.value)}
							key={option.value}
							className={`${styles.option} ${selectedOptions.includes(option.value) ? styles.selected : ""}`}
							onClick={() => handleOptionClick(option.value)}
						>
							{option.label}
						</div>
					))}
				</div>
			)}
		</div>
	);
};

export default FilterButton;
