import { useState } from "react";
import type { GostChanges } from "../../entities/gost/gostModel.ts";
import { useLazyGetChangesStatsQuery } from "../../features/api/apiSlice";
import { Input, RadioGroup } from "../../shared/components";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import styles from "./ChangesStatisticForm.module.scss";

interface ChangesStatisticFormProps {
	handleSubmit: (data: GostChanges[]) => void;
	startDateSubmit: (date: string) => void;
	endDateSubmit: (date: string) => void;
}

const ChangesStatisticForm = (props: ChangesStatisticFormProps) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;

	const date = new Date();
	const [changesData, setChangesData] = useState({
		status: "Valid",
		dateFrom: date.toISOString(),
		dateTo: date.toISOString(),
	});

	const [trigger] = useLazyGetChangesStatsQuery();

	const validateData = (event: React.FormEvent) => {
		event.preventDefault();
		trigger({
			status: changesData.status,
			StartDate: new Date(changesData.dateFrom).toISOString(),
			EndDate: new Date(changesData.dateTo).toISOString(),
		}).then((res) => {
			if (res.data) {
				handleSubmit(res.data);
				startDateSubmit(changesData.dateFrom);
				endDateSubmit(changesData.dateTo);
			}
		});
	};

	return (
		<form className={styles.form} onSubmit={(event) => validateData(event)}>
			<h2>Запрос статистики изменений</h2>
			<p className={styles.status}>Статус</p>
			<RadioGroup
				buttons={[
					{ id: "Valid", value: "Valid", label: "Действующий" },
					{ id: "Replaced", value: "Replaced", label: "Заменен" },
					{ id: "Canceled", value: "Canceled", label: "Отменен" },
				]}
				name="status"
				value={changesData.status}
				onChange={(value: string) => {
					setChangesData({ ...changesData, status: value });
				}}
			/>
			<p>Начальная дата изменений</p>
			<Input
				type="date"
				required={true}
				value={changesData.dateFrom}
				onChange={(value: string) => setChangesData({ ...changesData, dateFrom: value })}
			/>
			<p>Конечная дата изменений</p>
			<Input
				type="date"
				required={true}
				value={changesData.dateTo}
				onChange={(value: string) => setChangesData({ ...changesData, dateTo: value })}
			/>
			<UrfuButton type="submit">Сформировать отчёт</UrfuButton>
		</form>
	);
};

export default ChangesStatisticForm;