import CloseIcon from "@mui/icons-material/Close";
import { TextField } from "@mui/material";
import classNames from "classnames";
import { type ChangeEvent, useRef, useState } from "react";
import { Button, Input, RadioGroup } from "../../shared/components";
import IconButton from "../../shared/components/IconButton";
import TextArea from "../../shared/components/TextArea";
import styles from "./GostForm.module.scss";
import type { GostToSave } from "./newGostModel.ts";

interface GostFormProps {
	handleSubmit: (gost: GostToSave, file: File) => void;
	handleUploadFile: (file: File, docId: string | undefined) => void;
	gost?: GostToSave;
}

function getGostStub() {
	return {
		designation: "",
		fullName: "",
		codeOks: "",
		activityField: "",
		acceptanceYear: "",
		commissionYear: "",
		author: "",
		acceptedFirstTimeOrReplaced: "",
		content: "",
		keyWords: "",
		applicationArea: "",
		adoptionLevel: 0,
		documentText: "",
		changes: "",
		amendments: "",
		status: 0,
		harmonization: 1,
		isPrimary: true,
		references: [],
	} as GostToSave;
}

const GostForm = ({ handleSubmit, gost }: GostFormProps) => {
	const [newGost, setNewGost] = useState<GostToSave>(gost ?? getGostStub());
	const [reference, setReference] = useState("");
	const [file, setFile] = useState<File | null>(null);
	const [references, setReferences] = useState<string[]>(gost?.references ?? []);
	const ref = useRef<HTMLInputElement>(null);

	function handleLinks() {
		if (reference.length !== 0 && !references.includes(reference)) {
			setReferences((prevReferences) => [...prevReferences, reference]);
			setReference("");
		}
	}

	const handleFileUpload = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		console.log(event);
		//FIXME: fix type
		const target = event.target as HTMLInputElement;
		if (!target.files) return;
		const file = target.files[0];

		setFile(file);
	};

	function submit() {
		if (!file) {
			console.error("No file selected, aborting");
		} else {
			handleSubmit({ ...newGost, references: references }, file);
		}
	}

	return (
		<form>
			<table className={styles.gostTable}>
				<tbody>
					<tr>
						<td>Наименование стандарта</td>
						<td>
							<Input
								type="text"
								value={newGost.designation}
								onChange={(value: string) => setNewGost({ ...newGost, designation: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Заглавие стандарта</td>
						<td>
							<Input
								type="text"
								value={newGost.fullName}
								onChange={(value: string) => setNewGost({ ...newGost, fullName: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Код ОКС</td>
						<td>
							<Input
								type="text"
								value={newGost.codeOks}
								onChange={(value: string) => setNewGost({ ...newGost, codeOks: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Сфера деятельности</td>
						<td>
							<TextArea
								value={newGost.activityField}
								onChange={(value: string) => setNewGost({ ...newGost, activityField: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Год принятия</td>
						<td>
							<Input
								type="text"
								value={newGost.acceptanceYear}
								onChange={(value: string) => setNewGost({ ...newGost, acceptanceYear: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Год введения</td>
						<td>
							<Input
								type="text"
								value={newGost.commissionYear}
								onChange={(value: string) => setNewGost({ ...newGost, commissionYear: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Разработчик</td>
						<td>
							<Input
								type="text"
								value={newGost.author}
								onChange={(value: string) => setNewGost({ ...newGost, author: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Принят впервые/взамен</td>
						<td>
							<Input
								type="text"
								value={newGost.acceptedFirstTimeOrReplaced}
								onChange={(value: string) => setNewGost({ ...newGost, acceptedFirstTimeOrReplaced: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Содержание</td>
						<td>
							<TextArea
								value={newGost.content}
								onChange={(value: string) => setNewGost({ ...newGost, content: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Область применения</td>
						<td>
							<TextArea
								value={newGost.applicationArea}
								onChange={(value: string) => setNewGost({ ...newGost, applicationArea: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Ключевые слова</td>
						<td>
							<TextArea
								value={newGost.keyWords}
								onChange={(value: string) => setNewGost({ ...newGost, keyWords: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Уровень принятия</td>
						<td>
							<RadioGroup
								buttons={[
									{ id: "International", value: "0", label: "Международный" },
									{ id: "Foreign", value: "1", label: "Иностранный" },
									{ id: "Regional", value: "2", label: "Региональный" },
									{
										id: "Organizational",
										value: "3",
										label: "Организационный",
									},
									{ id: "National", value: "4", label: "Национальный" },
									{ id: "Interstate", value: "5", label: "Межгосударственный" },
								]}
								name="adoptionLevel"
								value={newGost.adoptionLevel.toString()}
								onChange={(value: string) => {
									setNewGost({
										...newGost,
										adoptionLevel: Number.parseInt(value),
									});
								}}
							/>
						</td>
					</tr>
					<tr>
						<td>Текст стандарта</td>
						<td>
							<TextArea
								value={newGost.documentText}
								onChange={(value: string) => setNewGost({ ...newGost, documentText: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Файл текста стандарта</td>
						<td>
							<TextField type="file" onChange={(event) => handleFileUpload(event)} />
						</td>
					</tr>
					<tr>
						<td>Нормативные ссылки</td>
						<td>
							<div className={styles.referencesContainer} onClick={() => ref.current?.focus()}>
								<Input
									type="text"
									value={reference}
									onChange={(value: string) => setReference(value)}
									className={styles.referencesInput}
								/>
								<Button onClick={() => handleLinks()} className="baseButton filledButton">
									Добавить
								</Button>
							</div>
							<ul className={styles.acceptedLinks}>
								{references?.map((reference) => {
									return (
										<li key={reference} className={classNames(styles.acceptedLink)}>
											{reference}
											<IconButton
												onClick={() => setReferences(references.filter((ref) => ref !== reference))}
												className="baseButton filledButton"
											>
												<CloseIcon />
											</IconButton>
										</li>
									);
								})}
							</ul>
						</td>
					</tr>
					<tr>
						<td>Изменения</td>
						<td>
							<Input
								type="text"
								value={newGost.changes}
								onChange={(value: string) => setNewGost({ ...newGost, changes: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Поправки</td>
						<td>
							<Input
								type="text"
								value={newGost.amendments}
								onChange={(value: string) => setNewGost({ ...newGost, amendments: value })}
							/>
						</td>
					</tr>
					<tr>
						<td>Отменен/Заменен/Действующий</td>
						<td>
							<RadioGroup
								buttons={[
									{ id: "Cancelled", value: "1", label: "Отменен" },
									{ id: "Replaced", value: "2", label: "Заменен" },
									{ id: "Current", value: "0", label: "Действующий" },
								]}
								name="status"
								value={newGost.status.toString()}
								onChange={(value: string) => {
									setNewGost({ ...newGost, status: Number.parseInt(value) });
								}}
							/>
						</td>
					</tr>
					<tr>
						<td>Уровень гармонизации</td>
						<td className={styles.radioButtons}>
							<RadioGroup
								buttons={[
									{
										id: "unharmonized",
										value: "0",
										label: "Негармонизированный",
									},
									{ id: "harmonized", value: "2", label: "Гармонизированный" },
									{ id: "modified", value: "1", label: "Модифицированный" },
								]}
								name="harmonization"
								value={newGost.harmonization.toString()}
								onChange={(value: string) => {
									setNewGost({
										...newGost,
										harmonization: Number.parseInt(value),
									});
								}}
							/>
						</td>
					</tr>
				</tbody>
			</table>
			<Button onClick={() => submit()} isFilled className={styles.saveButton}>
				Сохранить
			</Button>
		</form>
	);
};

export default GostForm;
