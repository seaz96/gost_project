import type {GostChanges} from "../../entities/gost/gostModel.ts";
import {GenericTable} from "../GenericTable/GenericTable.tsx";

interface ChangesStatisticTableProps {
	changesData: GostChanges[];
}

enum actions {
	Update = "Изменение",
	Create = "Создание",
}

const ChangesStatisticTable = (props: ChangesStatisticTableProps) => {
	const { changesData } = props;

	const columns = [
		{ header: "ID", accessor: (row: GostChanges) => row.documentId },
		{ header: "Обозначение", accessor: (row: GostChanges) => row.designation },
		{ header: "Наименование", accessor: (row: GostChanges) => row.fullName },
		{ header: "Действие", accessor: (row: GostChanges) => actions[row.action] },
		{ header: "Дата", accessor: (row: GostChanges) => formatDate(new Date(row.date)) },
	];

	return <GenericTable columns={columns} data={changesData} rowKey={"date"} />;
};

const formatDate = (date: Date) => {
	let day = date.getDate().toString();
	day = day.length === 1 ? `0${day}` : day;
	const month = date.getMonth() + 1;
	const year = date.getFullYear();
	return `${day}.${month}.${year}`;
};

export default ChangesStatisticTable;