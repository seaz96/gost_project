import { zodResolver } from "@hookform/resolvers/zod";
import CloseIcon from "@mui/icons-material/Close";
import { useState } from "react";
import { useForm } from "react-hook-form";
import UrfuTextInput from "shared/components/Input/UrfuTextInput";
import type { GostAddModel, GostRequestModel } from "../../entities/gost/gostModel";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuTextArea from "../../shared/components/Input/UrfuTextArea.tsx";
import UrfuRadioGroup from "../../shared/components/RadioGroup/UrfuRadioGroup";
import styles from "./GostForm.module.scss";
import { type GostFormValues, gostFormSchema } from "./types";

interface GostFormProps {
	handleSubmit: (gost: GostAddModel) => void;
	data?: GostRequestModel;
}

const adoptionLevelOptions = [
	{ value: "International", label: "Международный" },
	{ value: "Foreign", label: "Иностранный" },
	{ value: "Regional", label: "Региональный" },
	{ value: "Organizational", label: "Организационный" },
	{ value: "National", label: "Национальный" },
	{ value: "Interstate", label: "Межгосударственный" },
];

const statusOptions = [
	{ value: "Valid", label: "Действующий" },
	{ value: "Canceled", label: "Отменен" },
	{ value: "Replaced", label: "Заменен" },
	{ value: "Inactive", label: "Неактивен" },
];

const harmonizationOptions = [
	{ value: "Unharmonized", label: "Негармонизированный" },
	{ value: "Harmonized", label: "Гармонизированный" },
	{ value: "Modified", label: "Модифицированный" },
];

export default function GostForm({ handleSubmit, data }: GostFormProps) {
	const [newReference, setNewReference] = useState("");

	const {
		register,
		handleSubmit: handleFormSubmit,
		formState: { errors },
		watch,
		setValue,
	} = useForm<GostFormValues>({
		resolver: zodResolver(gostFormSchema),
		defaultValues: data || { references: [] },
	});

	const references = watch("references") || [];

	const onSubmit = (data: GostFormValues) => {
		console.log(data);
		if (!data.file && !data.documentText) {
			return;
		}
		handleSubmit(data as GostAddModel);
	};

	const handleAddReference = () => {
		if (newReference && !references.includes(newReference)) {
			setValue("references", [...references, newReference]);
			setNewReference("");
		}
	};

	const handleRemoveReference = (reference: string) => {
		setValue(
			"references",
			references.filter((ref) => ref !== reference),
		);
	};

	return (
		<form onSubmit={handleFormSubmit(onSubmit)} className={styles.form}>
			<div className={styles.formGrid}>
				<UrfuTextInput
					{...register("designation")}
					label="Наименование стандарта"
					error={errors.designation?.message}
				/>
				<UrfuTextInput {...register("fullName")} label="Заглавие стандарта" error={errors.fullName?.message} />
				<UrfuTextInput {...register("codeOks")} label="Код ОКС" error={errors.codeOks?.message} />
				<UrfuTextInput
					{...register("activityField")}
					label="Сфера деятельности"
					error={errors.activityField?.message}
				/>
				<UrfuTextInput
					{...register("acceptanceYear", { valueAsNumber: true })}
					type="number"
					label="Год принятия"
					error={errors.acceptanceYear?.message}
				/>
				<UrfuTextInput
					{...register("commissionYear", { valueAsNumber: true })}
					type="number"
					label="Год введения"
					error={errors.commissionYear?.message}
				/>
				<UrfuTextInput {...register("author")} label="Разработчик" error={errors.author?.message} />
				<UrfuTextInput
					{...register("acceptedFirstTimeOrReplaced")}
					label="Принят впервые или заменен"
					error={errors.acceptedFirstTimeOrReplaced?.message}
				/>
				<UrfuTextArea {...register("content")} label="Содержание" error={errors.content?.message} rows={4} />
				<UrfuTextArea
					{...register("applicationArea")}
					label="Область применения"
					error={errors.applicationArea?.message}
					rows={4}
				/>
				<UrfuTextArea {...register("keyWords")} label="Ключевые слова" error={errors.keyWords?.message} rows={4} />
				<UrfuTextArea
					{...register("documentText")}
					label="Текст стандарта"
					error={errors.documentText?.message}
					rows={4}
				/>
				<div className={styles.fileUpload}>
					<input type="file" {...register("file")} accept=".pdf,.doc,.docx" />
				</div>
				<div className={styles.fullWidth}>
					<UrfuRadioGroup
						{...register("adoptionLevel")}
						label="Уровень принятия"
						options={adoptionLevelOptions}
						error={errors.adoptionLevel?.message}
						value={watch("adoptionLevel")}
					/>
				</div>
				<div className={styles.fullWidth}>
					<UrfuRadioGroup
						{...register("status")}
						label="Статус"
						options={statusOptions}
						error={errors.status?.message}
						value={watch("status")}
					/>
				</div>
				<div className={styles.fullWidth}>
					<UrfuRadioGroup
						{...register("harmonization")}
						label="Уровень гармонизации"
						options={harmonizationOptions}
						error={errors.harmonization?.message}
						value={watch("harmonization")}
					/>
				</div>
				<div className={styles.references}>
					<div className={styles.referenceInput}>
						<UrfuTextInput
							value={newReference}
							onChange={(e) => setNewReference(e.target.value)}
							label="Нормативные ссылки"
							error={errors.references?.message}
						/>
						<UrfuButton onClick={handleAddReference}>Добавить</UrfuButton>
					</div>
					<div className={styles.referenceList}>
						{references.map((reference) => (
							<div key={reference} className={styles.referenceItem}>
								<span>{reference}</span>
								<button type="button" onClick={() => handleRemoveReference(reference)} className={styles.removeButton}>
									<CloseIcon />
								</button>
							</div>
						))}
					</div>
				</div>
				<UrfuTextInput {...register("changes")} label="Изменения" error={errors.changes?.message} />
				<UrfuTextInput {...register("amendments")} label="Поправки" error={errors.amendments?.message} />
			</div>

			<div className={styles.submitButton}>
				<UrfuButton disabled={Object.keys(errors).length > 0} type="submit">
					Сохранить
				</UrfuButton>
			</div>
		</form>
	);
}