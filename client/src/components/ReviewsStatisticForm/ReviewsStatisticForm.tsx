import { useForm } from "react-hook-form";
import type { GostViews } from "../../entities/gost/gostModel.ts";
import { useLazyGetViewsStatsQuery } from "../../features/api/apiSlice";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput.tsx";
import styles from "./ReviewsStatisticForm.module.scss";

interface ReviewsStatisticFormProps {
	handleSubmit: (data: GostViews[]) => void;
	startDateSubmit: (date: string) => void;
	endDateSubmit: (date: string) => void;
}

const ReviewsStatisticForm = (props: ReviewsStatisticFormProps) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;
	const { register, handleSubmit: handleFormSubmit } = useForm({
		defaultValues: {
			designation: "",
			codeOks: "",
			activityField: "",
			startDate: "",
			endDate: "",
		},
	});

	const [trigger] = useLazyGetViewsStatsQuery();

	interface FormData {
		designation: string;
		codeOks: string;
		activityField: string;
		startDate: string;
		endDate: string;
	}

	const onSubmit = (data: FormData) => {
		const startDate = data.startDate ? new Date(data.startDate).toISOString() : new Date("1970-01-01").toISOString();
		const endDate = data.endDate ? new Date(data.endDate).toISOString() : new Date().toISOString();

		trigger({
			...data,
			startDate: startDate,
			endDate: endDate,
		}).then((res) => {
			if (res.data) {
				handleSubmit(res.data);
				startDateSubmit(startDate);
				endDateSubmit(endDate);
			}
		});
	};

	return (
		<form className={styles.form} onSubmit={handleFormSubmit(onSubmit)}>
			<h2>Запрос статистики обращений</h2>
			<UrfuTextInput label="Название ГОСТа" type="text" {...register("designation")} />
			<UrfuTextInput label="Код ОКС" type="text" {...register("codeOks")} />
			<UrfuTextInput label="Сфера деятельности" type="text" {...register("activityField")} />
			<p>Начальная дата</p>
			<UrfuTextInput type="date" {...register("startDate")} />
			<p>Конечная дата</p>
			<UrfuTextInput type="date" {...register("endDate")} />
			<UrfuButton type="submit">Сформировать отчёт</UrfuButton>
		</form>
	);
};

export default ReviewsStatisticForm;