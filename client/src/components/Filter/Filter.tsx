import {Search} from "@mui/icons-material";
import { useState } from "react";
import type { GostSearchParams } from "../../entities/gost/gostModel.ts";
import IconButton from "../../shared/components/IconButton";
import styles from "./Filter.module.scss";

interface FilterProps {
	filterSubmit: Function;
}

const Filter: React.FC<FilterProps> = (props) => {
	const { filterSubmit } = props;
	const [filterData, setFilterData] = useState<Partial<GostSearchParams> & { text?: string }>({});

	const handleSubmit = () => {
		filterSubmit(filterData);
	};

	return (
		<div className={styles.filterContainer}>
			<input
				type="text"
				className={styles.input}
				value={filterData.text}
				onChange={(event) => setFilterData({ ...filterData, text: event.target.value })}
				placeholder="Поиск по обозначению или наименованию..."
			/>
			<div className={styles.buttonsContainer}>
				<IconButton onClick={() => handleSubmit()} isFilled className={styles.searchButton}>
					<Search />
				</IconButton>
			</div>
		</div>
	);
};

export default Filter;