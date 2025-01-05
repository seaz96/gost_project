import type React from "react";
import { useState } from "react";
import { Button, Input, RadioGroup } from "../../shared/components";

import { axiosInstance } from "../../shared/configs/axiosConfig.ts";
import styles from "./ChangesStatisticForm.module.scss";

interface ChangesStatisticFormProps {
	handleSubmit: Function;
	startDateSubmit: Function;
	endDateSubmit: Function;
}

const ChangesStatisticForm: React.FC<ChangesStatisticFormProps> = (props) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;

	const [changesData, setChangesData] = useState({
		status: 0 | 1 | 2,
		dateFrom: "",
		dateTo: "",
		count: 0,
	});

	const validateData = (event: React.FormEvent) => {
		event.preventDefault();
		axiosInstance
			.get("/stats/get-count", {
				params: {
					status: changesData.status,
					count: changesData.count,
					StartDate: new Date(changesData.dateFrom).toISOString(),
					EndDate: new Date(changesData.dateTo).toISOString(),
				},
			})
			.then((response) => {
				handleSubmit(response.data);
				startDateSubmit(changesData.dateFrom);
				endDateSubmit(changesData.dateTo);
			});

		handleSubmit();
	};

	return (
		<form className={styles.form} onSubmit={(event) => validateData(event)}>
			<p className={styles.status}>Статус</p>
			<RadioGroup
				buttons={[
					{ id: "Cancelled", value: "1", label: "Отменен" },
					{ id: "Replaced", value: "2", label: "Заменен" },
					{ id: "Current", value: "0", label: "Действующий" },
				]}
				name="status"
				value={changesData.status.toString()}
				onChange={(value: string) => {
					setChangesData({ ...changesData, status: Number.parseInt(value) });
				}}
			/>
			<p className={styles.datesTitle}>Даты обращение к карте(от, до)</p>
			<Input
				placeholder="От..."
				type="date"
				value={changesData.dateFrom}
				onChange={(value: string) => setChangesData({ ...changesData, dateFrom: value })}
			/>
			<Input
				placeholder="До..."
				type="date"
				value={changesData.dateTo}
				onChange={(value: string) => setChangesData({ ...changesData, dateTo: value })}
			/>
			<p className={styles.countTitle}>Количетсво выводимых изменений</p>
			<Input
				placeholder="Количество..."
				type="text"
				value={changesData.count}
				onChange={(value: string) => setChangesData({ ...changesData, count: +value })}
			/>
			<Button onClick={() => {}} className={styles.formButton} isFilled type="submit">
				Сформировать отчёт
			</Button>
		</form>
	);
};

export default ChangesStatisticForm;