import {type FormEvent, useState} from "react";
import type {GostViews} from "../../entities/gost/gostModel.ts";
import {useLazyGetViewsStatsQuery} from "../../features/api/apiSlice";
import {Input} from "../../shared/components";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import styles from "./ReviewsStatisticForm.module.scss";

interface ReviewsStatisticFormProps {
	handleSubmit: (data: GostViews[]) => void;
	startDateSubmit: (date: string) => void;
	endDateSubmit: (date: string) => void;
}

const ReviewsStatisticForm = (props: ReviewsStatisticFormProps) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;

	const [reviewsData, setReviewsData] = useState({
		designation: "",
		codeOks: "",
		activityField: "",
		startDate: "",
		endDate: "",
	});

	const [trigger, { data }] = useLazyGetViewsStatsQuery();

	const validateData = (event: FormEvent) => {
		event.preventDefault();

		const startDate = reviewsData.startDate
			? new Date(reviewsData.startDate).toISOString()
			: new Date("1970-01-01").toISOString();
		const endDate = reviewsData.endDate ? new Date(reviewsData.endDate).toISOString() : new Date().toISOString();

		trigger({
			...reviewsData,
			startDate: startDate,
			endDate: endDate,
		}).then((res) => {
			if (res.data) {
				handleSubmit(res.data);
				startDateSubmit(startDate);
				endDateSubmit(endDate);
			}
		});

		if (data) {
			handleSubmit(data);
			startDateSubmit(startDate);
			endDateSubmit(endDate);
		}
	};

	return (
		<form className={styles.form} onSubmit={(event) => validateData(event)}>
			<h2>Запрос статистики обращений</h2>
			<Input
				label="Название ГОСТа"
				type="text"
				value={reviewsData.designation}
				onChange={(value: string) => setReviewsData({ ...reviewsData, designation: value })}
			/>
			<Input
				label="Код ОКС"
				type="text"
				value={reviewsData.codeOks}
				onChange={(value: string) => setReviewsData({ ...reviewsData, codeOks: value })}
			/>
			<Input
				label="Сфера деятельности"
				type="text"
				value={reviewsData.activityField}
				onChange={(value: string) => setReviewsData({ ...reviewsData, activityField: value })}
			/>
			<p className={styles.datesTitle}>Даты обращение к карте(от, до)</p>
			<Input
				placeholder="От..."
				type="date"
				value={reviewsData.startDate}
				onChange={(value: string) => setReviewsData({ ...reviewsData, startDate: value })}
			/>
			<Input
				placeholder="До..."
				type="date"
				value={reviewsData.endDate}
				onChange={(value: string) => setReviewsData({ ...reviewsData, endDate: value })}
			/>
			<UrfuButton type="submit">Сформировать отчёт</UrfuButton>
		</form>
	);
};

export default ReviewsStatisticForm;