import { Collapse } from "@mui/material";
import classNames from "classnames";
import { useState } from "react";
import type { GostSearchParams } from "../../../entities/gost/gostModel.ts";
import { Input } from "../../../shared/components";
import arrowDown from "../assets/arrowDown.png";
import styles from "./FilterDropdown.module.scss";

interface FilterDropdownProps {
	filterData: Partial<GostSearchParams>;
	filterSubmit: (filterData: Partial<GostSearchParams>) => void;
}

const FilterDropdown: React.FC<FilterDropdownProps> = (props) => {
	const { filterSubmit, filterData } = props;

	const [filterStatus, setFilterStatus] = useState({
		designation: "",
		fullName: "",
		codeOks: false,
		activityField: false,
		acceptanceYear: false,
		commissionYear: false,
		author: false,
		acceptedFirstTimeOrReplaced: false,
		content: false,
		keyWords: false,
		applicationArea: false,
		adoptionLevel: false,
		documentText: false,
		changes: false,
		amendments: false,
		status: false,
		harmonization: false,
		isPrimary: true,
		referencesId: false,
	});

	return (
		<div className={styles.dropdown}>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() => setFilterStatus({ ...filterStatus, codeOks: !filterStatus.codeOks })}
				>
					<img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.codeOks ? styles.arrowUp : "")} />
					<p className={styles.dropdownItemName}>Код ОКС</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.codeOks}>
					<Input
						type="text"
						value={filterData.SearchFilters?.CodeOks}
						onChange={(value: string) =>
							filterSubmit({
								...filterData,
								SearchFilters: { ...filterData.SearchFilters, CodeOks: value },
							})
						}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							acceptanceYear: !filterStatus.acceptanceYear,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.acceptanceYear ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Год принятия</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.acceptanceYear}>
					<Input
						type="text"
						value={filterData.acceptanceYear}
						onChange={(value: string) => filterSubmit({ ...filterData, acceptanceYear: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							commissionYear: !filterStatus.commissionYear,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.commissionYear ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Год введения</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.commissionYear}>
					<Input
						type="text"
						value={filterData.commissionYear}
						onChange={(value: string) => filterSubmit({ ...filterData, commissionYear: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() => setFilterStatus({ ...filterStatus, author: !filterStatus.author })}
				>
					<img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.author ? styles.arrowUp : "")} />
					<p className={styles.dropdownItemName}>Разработчик</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.author}>
					<Input
						type="text"
						value={filterData.author}
						onChange={(value: string) => filterSubmit({ ...filterData, author: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							acceptedFirstTimeOrReplaced: !filterStatus.acceptedFirstTimeOrReplaced,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.acceptedFirstTimeOrReplaced ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Принят впервые/взамен</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.acceptedFirstTimeOrReplaced}>
					<Input
						type="text"
						value={filterData.acceptedFirstTimeOrReplaced}
						onChange={(value: string) =>
							filterSubmit({
								...filterData,
								acceptedFirstTimeOrReplaced: value,
							})
						}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() => setFilterStatus({ ...filterStatus, content: !filterStatus.content })}
				>
					<img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.content ? styles.arrowUp : "")} />
					<p className={styles.dropdownItemName}>Содержание</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.content}>
					<Input
						type="text"
						value={filterData.content}
						onChange={(value: string) => filterSubmit({ ...filterData, content: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							applicationArea: !filterStatus.applicationArea,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.applicationArea ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Область применения</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.applicationArea}>
					<Input
						type="text"
						value={filterData.applicationArea}
						onChange={(value: string) => filterSubmit({ ...filterData, applicationArea: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							keyWords: !filterStatus.keyWords,
						})
					}
				>
					<img src={arrowDown} className={classNames(styles.arrowDown, filterStatus.keyWords ? styles.arrowUp : "")} />
					<p className={styles.dropdownItemName}>Ключевые слова</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.keyWords}>
					<Input
						type="text"
						value={filterData.keyWords}
						onChange={(value: string) => filterSubmit({ ...filterData, keyWords: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							adoptionLevel: !filterStatus.adoptionLevel,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.adoptionLevel ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Уровень принятия</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.adoptionLevel}></Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							documentText: !filterStatus.documentText,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.documentText ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Текст стандарта</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.documentText}>
					<Input
						type="text"
						value={filterData.documentText}
						onChange={(value: string) => filterSubmit({ ...filterData, documentText: value })}
					/>
				</Collapse>
			</div>
			<div className={styles.dropdownItem}>
				<div
					className={styles.dropdownItemInfo}
					onClick={() =>
						setFilterStatus({
							...filterStatus,
							harmonization: !filterStatus.harmonization,
						})
					}
				>
					<img
						src={arrowDown}
						className={classNames(styles.arrowDown, filterStatus.harmonization ? styles.arrowUp : "")}
					/>
					<p className={styles.dropdownItemName}>Уровень гармонизации</p>
				</div>
				<Collapse className={styles.dropdownItemFilter} in={filterStatus.harmonization}></Collapse>
			</div>
		</div>
	);
};

export default FilterDropdown;
