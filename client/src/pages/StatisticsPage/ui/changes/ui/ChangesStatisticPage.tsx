import {useEffect, useRef, useState} from "react";
import ChangesStatisticForm from "../../../../../components/ChangesStatisticForm/ChangesStatisticForm.tsx";
import ChangesStatisticTable from "../../../../../components/ChangesStatisticTable/ChangesStatisticTable.tsx";
import type {GostChanges} from "../../../../../entities/gost/gostModel.ts";
import styles from "./ChangesStatisticPage.module.scss";

const ChangesStatisticPage = () => {
	const [changesData, setChangesData] = useState<GostChanges[] | null>(null);
	const [startDate, setStartDate] = useState("");
	const [endDate, setEndDate] = useState("");
	const reportRef = useRef<HTMLElement>(null);

	const handleSubmit = (values: GostChanges[]) => {
		setChangesData(values);
	};

	useEffect(() => {
		if (reportRef.current && changesData) {
			reportRef.current.scrollIntoView({ behavior: "smooth" });
		}
	}, [changesData]);

	return (
		<section>
			<section>
				<ChangesStatisticForm
					handleSubmit={handleSubmit}
					startDateSubmit={setStartDate}
					endDateSubmit={setEndDate}
				/>
			</section>
			{changesData && (
				<section className={"verticalPadding"} ref={reportRef}>
					<h2 className={styles.title}>Отчёт об изменениях</h2>
					<p className="verticalPadding">
						{`с ${formatDate(new Date(startDate))} по ${formatDate(new Date(endDate))}`}
					</p>
					<div className="verticalPadding">
						<ChangesStatisticTable changesData={changesData} />
					</div>
				</section>
			)}
		</section>
	);
};

const formatDate = (date: Date) => {
	let day = date.getDate().toString();
	day = day.length === 1 ? `0${day}` : day;
	const month = date.getMonth() + 1;
	const year = date.getFullYear();
	return `${day}.${month}.${year}`;
};

export default ChangesStatisticPage;