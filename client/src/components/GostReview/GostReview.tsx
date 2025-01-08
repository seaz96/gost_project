import classNames from "classnames";
import { type ReactNode, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAppSelector } from "../../app/hooks.ts";
import { gostModel } from "../../entities/gost";
import type { GostFetchModel } from "../../entities/gost/gostModel.ts";
import { useChangeGostStatusMutation, useDeleteGostMutation } from "../../features/api/apiSlice";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import { GenericTable } from "../GenericTable/GenericTable";
import Modal from "../Modal/Modal.tsx";
import styles from "./GostReview.module.scss";

interface GostReviewProps {
	gost: GostFetchModel;
	gostId: number;
}

interface GostComparisonRow {
	id: string;
	field: string;
	primary: ReactNode;
	actual: ReactNode;
}

const GostReview = (props: GostReviewProps) => {
	const { gost, gostId } = props;
	const navigate = useNavigate();
	const user = useAppSelector((s) => s.user.user);
	const [deleteModalOpen, setDeleteModalOpen] = useState(false);
	const [cancelModalOpen, setCancelModalOpen] = useState(false);
	const [recoverModalOpen, setRecoverModalOpen] = useState(false);

	const [deleteGost] = useDeleteGostMutation();
	const [changeStatus] = useChangeGostStatusMutation();

	const onDeleteSubmit = async () => {
		await deleteGost(gostId.toString());
		navigate("/");
	};

	const recoverDoc = async () => {
		await changeStatus({ id: gostId, status: 0 });
		navigate("/");
	};

	const cancelDoc = async () => {
		await changeStatus({ id: gostId, status: 1 });
		navigate("/");
	};

	const renderReferences = (refs: typeof gost.references) => (
		<>
			{refs.map((ref, index) =>
				ref.status === "Inactive" ? (
					<p key={`${ref.id}-${index}`}>{ref.designation}</p>
				) : (
					<Link key={`${ref.id}-${index}`} to={`/gost-review/${ref.id}`}>
						<p
							className={classNames(
								ref.status === "Valid" && styles.activeRef,
								(ref.status === "Canceled" || ref.status === "Replaced") && styles.oldRef,
							)}
						>
							{ref.designation}
						</p>
					</Link>
				),
			)}
		</>
	);

	const tableData: GostComparisonRow[] = [
		{ id: "name", field: "Наименование стандарта", primary: gost.primary.fullName, actual: gost.actual.fullName },
		{ id: "codeOks", field: "Код ОКС", primary: gost.primary.codeOks, actual: gost.actual.codeOks },
		{
			id: "activityField",
			field: "Сфера деятельности",
			primary: gost.primary.activityField,
			actual: gost.actual.activityField,
		},
		{
			id: "acceptanceYear",
			field: "Год принятия",
			primary: gost.primary.acceptanceYear,
			actual: gost.actual.acceptanceYear,
		},
		{
			id: "commissionYear",
			field: "Год введения",
			primary: gost.primary.commissionYear,
			actual: gost.actual.commissionYear,
		},
		{ id: "author", field: "Разработчик", primary: gost.primary.author, actual: gost.actual.author },
		{
			id: "firstOrReplaced",
			field: "Принят впервые/взамен",
			primary: gost.primary.acceptedFirstTimeOrReplaced,
			actual: gost.actual.acceptedFirstTimeOrReplaced,
		},
		{ id: "content", field: "Содержание", primary: gost.primary.content, actual: gost.actual.content },
		{
			id: "applicationArea",
			field: "Область применения",
			primary: <div style={{ whiteSpace: "pre-line" }}>{gost.primary.applicationArea}</div>,
			actual: <div style={{ whiteSpace: "pre-line" }}>{gost.actual.applicationArea}</div>,
		},
		{ id: "keyWords", field: "Ключевые слова", primary: gost.primary.keyWords, actual: gost.actual.keyWords },
		{
			id: "adoptionLevel",
			field: "Уровень принятия",
			primary: gostModel.AdoptionLevelToRu[gost.primary.adoptionLevel],
			actual: gostModel.AdoptionLevelToRu[gost.actual.adoptionLevel],
		},
		{
			id: "documentText",
			field: "Текст стандарта",
			primary: <a href={gost.primary.documentText}>{gost.primary.documentText}</a>,
			actual: <a href={gost.actual.documentText}>{gost.actual.documentText}</a>,
		},
		{ id: "changes", field: "Изменения", primary: gost.primary.changes, actual: gost.actual.changes },
		{ id: "amendments", field: "Поправки", primary: gost.primary.amendments, actual: gost.actual.amendments },
		{
			//TODO: Уточнить о status/harmonization в primary/actual
			id: "status",
			field: "Действующий/Отменён/Заменён",
			primary: gostModel.StatusToRu[gost.status],
			actual: gostModel.StatusToRu[gost.status],
		},
		{
			id: "harmonization",
			field: "Уровень гармонизации",
			primary: gostModel.HarmonizationToRu[gost.primary.harmonization],
			actual: gostModel.HarmonizationToRu[gost.actual.harmonization],
		},
	];

	const columns = [
		{ header: "Поле", accessor: (row: GostComparisonRow) => row.field },
		{ header: "Первоначальное значение", accessor: (row: GostComparisonRow) => row.primary },
		{ header: "Последняя актуализация", accessor: (row: GostComparisonRow) => row.actual ?? "-" },
	];

	//TODO: buttons to links
	return (
		<>
			<main className={styles.reviewContainer}>
				<h1 className="verticalPadding">Просмотр документа {gost.primary.designation}</h1>
				{(user?.role === "Admin" || user?.role === "Heisenberg") && (
					<div className={styles.buttonsContainer}>
						<UrfuButton onClick={() => navigate(`/gost-edit/${gostId}`)} size={"small"} outline={true}>
							Редактировать
						</UrfuButton>
						<UrfuButton onClick={() => setDeleteModalOpen(true)} size={"small"} outline={true}>
							Удалить
						</UrfuButton>
						{gost.status === "Canceled" ? (
							<UrfuButton onClick={() => setRecoverModalOpen(true)} size={"small"} outline={true}>
								Восстановить
							</UrfuButton>
						) : (
							<UrfuButton onClick={() => setCancelModalOpen(true)} size={"small"} outline={true}>
								Отменить
							</UrfuButton>
						)}
						<UrfuButton onClick={() => navigate(`/gost-replace-page/${gostId}`)} size={"small"} outline={true}>
							Заменить
						</UrfuButton>
						<UrfuButton onClick={() => navigate(`/gost-actualize-page/${gostId}`)} size={"small"} outline={true}>
							Актуализировать
						</UrfuButton>
					</div>
				)}
				<GenericTable columns={columns} data={tableData} rowKey="id" />
				<h2 className="verticalPadding">Нормативные ссылки</h2>
				{gost.references.length > 0 ? (
					<div className={styles.references}>{renderReferences(gost.references)}</div>
				) : (
					<p>Нет нормативных ссылок</p>
				)}
			</main>
			<DeleteCard isOpen={deleteModalOpen} setIsOpen={setDeleteModalOpen} onSubmitFunction={onDeleteSubmit} />
			<CancelCard isOpen={cancelModalOpen} setIsOpen={setCancelModalOpen} onSubmitFunction={cancelDoc} />
			<RecoverCard isOpen={recoverModalOpen} setIsOpen={setRecoverModalOpen} onSubmitFunction={recoverDoc} />
		</>
	);
};

interface DeleteCardProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	onSubmitFunction: () => void;
}

const DeleteCard = (props: DeleteCardProps) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Modal
			isOpen={isOpen}
			setIsOpen={setIsOpen}
			title="Удалить ГОСТ?"
			description="Если вы удалите ГОСТ, он полностью удалится из базы без возможности восстановления"
			primaryActionText="Удалить"
			primaryAction={onSubmitFunction}
			secondaryActionText="Отменить"
		/>
	);
};

interface RecoverCardProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	onSubmitFunction: () => void;
}

const RecoverCard = (props: RecoverCardProps) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Modal
			isOpen={isOpen}
			setIsOpen={setIsOpen}
			title="Восстановить ГОСТ?"
			description="Если вы восстановите ГОСТ, он удалится из архива, а его статус будет изменён на 'Действующий'"
			primaryActionText="Восстановить"
			primaryAction={onSubmitFunction}
			secondaryActionText="Назад"
		/>
	);
};

interface CancelCardProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	onSubmitFunction: () => void;
}

const CancelCard = (props: CancelCardProps) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Modal
			isOpen={isOpen}
			setIsOpen={setIsOpen}
			title="Отменить ГОСТ?"
			description="Если вы отмените ГОСТ, он перенесется в архив, а его статус поменяться на 'Отменен'"
			primaryActionText="Отменить ГОСТ"
			primaryAction={onSubmitFunction}
			secondaryActionText="Назад"
		/>
	);
};

export default GostReview;