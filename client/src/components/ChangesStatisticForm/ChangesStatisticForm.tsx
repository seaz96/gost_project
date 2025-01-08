import { useForm } from "react-hook-form";
import type { GostChanges } from "../../entities/gost/gostModel";
import { useLazyGetChangesStatsQuery } from "../../features/api/apiSlice";
import UrfuButton from "../../shared/components/Button/UrfuButton";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput";
import UrfuRadioGroup from "../../shared/components/RadioGroup/UrfuRadioGroup";
import styles from "./ChangesStatisticForm.module.scss";

interface ChangesStatisticFormProps {
	handleSubmit: (data: GostChanges[]) => void;
	startDateSubmit: (date: string) => void;
	endDateSubmit: (date: string) => void;
}

const ChangesStatisticForm = (props: ChangesStatisticFormProps) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;
	const { register, handleSubmit: handleFormSubmit, watch, setValue, formState: {errors} } = useForm({
		defaultValues: {
			status: "Valid",
			dateFrom: new Date().toISOString(),
			dateTo: new Date().toISOString(),
		},
	});

	const [trigger] = useLazyGetChangesStatsQuery();

	const onSubmit = (data: { status: string; dateFrom: string; dateTo: string }) => {
		trigger({
			status: data.status,
			StartDate: new Date(data.dateFrom).toISOString(),
			EndDate: new Date(data.dateTo).toISOString(),
		}).then((res) => {
			if (res.data) {
				handleSubmit(res.data);
				startDateSubmit(data.dateFrom);
				endDateSubmit(data.dateTo);
			}
		});
	};

	return (
		<form className={styles.form} onSubmit={handleFormSubmit(onSubmit)}>
			<h2>Запрос статистики изменений</h2>
			<p className={styles.status}>Статус</p>
			<UrfuRadioGroup
				options={[
					{ value: "Valid", label: "Действующий" },
					{ value: "Replaced", label: "Заменен" },
					{ value: "Canceled", label: "Отменен" },
				]}
				name="status"
				value={watch("status")}
				onChange={(e) => {
					const { value } = e.target;
					setValue("status", value);
				}}
				ref={register("status").ref}
			/>
			<p>Начальная дата изменений</p>
			<UrfuTextInput
				type="date"
				error={errors.dateFrom?.message}
				{...register("dateFrom", { required: "Начальная дата обязательна" })}

			/>
			<p>Конечная дата изменений</p>
			<UrfuTextInput
				type="date"
				error={errors.dateTo?.message}
				{...register("dateTo", { required: "Конечная дата обязательна" })}
			/>
			<UrfuButton type="submit">Сформировать отчёт</UrfuButton>
		</form>
	);
};

export default ChangesStatisticForm;