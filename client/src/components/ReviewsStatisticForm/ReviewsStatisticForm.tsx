import { useState } from "react";
import { useGetViewsStatsQuery } from "../../features/api/apiSlice";
import { Button, Input } from "../../shared/components";
import styles from "./ReviewsStatisticForm.module.scss";

interface ReviewsStatisticFormProps {
	handleSubmit: Function;
	startDateSubmit: Function;
	endDateSubmit: Function;
}

const ReviewsStatisticForm: React.FC<ReviewsStatisticFormProps> = (props) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;

	const [reviewsData, setReviewsData] = useState({
		designation: "",
		codeOks: "",
		activityField: "",
		startDate: "",
		endDate: "",
	});

	const validateData = (event: React.FormEvent) => {
		event.preventDefault();

		const startDate = reviewsData.startDate
			? new Date(reviewsData.startDate).toISOString()
			: new Date("1970-01-01").toISOString();
		const endDate = reviewsData.endDate ? new Date(reviewsData.endDate).toISOString() : new Date().toISOString();

		const { data } = useGetViewsStatsQuery({
			...reviewsData,
			startDate,
			endDate,
		});

		if (data) {
			handleSubmit(data);
			startDateSubmit(startDate);
			endDateSubmit(endDate);
		}
	};

	return (
		<form className={styles.form} onSubmit={(event) => validateData(event)}>
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
			<Button onClick={() => {}} className={styles.formButton} isFilled type="submit">
				Сформировать отчёт
			</Button>
		</form>
	);
};

export default ReviewsStatisticForm;
