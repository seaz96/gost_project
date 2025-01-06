import {Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle} from "@mui/material";
import classNames from "classnames";
import {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import {useAppSelector} from "../../app/hooks.ts";
import {gostModel} from "../../entities/gost";
import {useChangeGostStatusMutation, useDeleteGostMutation} from "../../features/api/apiSlice";
import {Button} from "../../shared/components";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import {GenericTable} from "../GenericTable/GenericTable";
import styles from "./GostReview.module.scss";

interface GostReviewProps {
	gost: gostModel.Gost;
	gostId: number;
}

interface GostComparisonRow {
	id: string;
	field: string;
	primary: React.ReactNode;
	actual: React.ReactNode;
}

const GostReview: React.FC<GostReviewProps> = (props) => {
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
				ref.status === 3 ? (
					<p key={`${ref.docId}-${index}`}>{ref.designation}</p>
				) : (
					<Link key={`${ref.docId}-${index}`} to={`/gost-review/${ref.docId}`}>
						<p
							className={classNames(
								ref.status === 0 && styles.activeRef,
								(ref.status === 1 || ref.status === 2) && styles.oldRef,
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
			primary: gostModel.AdoptionLevel[gost.primary.adoptionLevel],
			actual: gostModel.AdoptionLevel[gost.actual.adoptionLevel],
		},
		{
			id: "documentText",
			field: "Текст стандарта",
			primary: <a href={gost.primary.documentText}>{gost.primary.documentText}</a>,
			actual: <a href={gost.actual.documentText}>{gost.actual.documentText}</a>,
		},
		{
			id: "references",
			field: "Нормативные ссылки",
			primary: renderReferences(gost.references),
			actual: renderReferences(gost.references),
		},
		{ id: "changes", field: "Изменения", primary: gost.primary.changes, actual: gost.actual.changes },
		{ id: "amendments", field: "Поправки", primary: gost.primary.amendments, actual: gost.actual.amendments },
		{
			id: "status",
			field: "Действующий/Отменён/Заменён",
			primary: gostModel.Statuses[gost.primary.status],
			actual: gostModel.Statuses[gost.actual.status],
		},
		{
			id: "harmonization",
			field: "Уровень гармонизации",
			primary: gostModel.Harmonization[gost.primary.harmonization],
			actual: gostModel.Harmonization[gost.actual.harmonization],
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
			<div className={styles.reviewContainer}>
				<h2 className={styles.title}>Просмотр документа {gost.primary.designation}</h2>
				{(user?.role === "Admin" || user?.role === "Heisenberg") && (
					<div className={styles.buttonsContainer}>
						<UrfuButton onClick={() => navigate(`/gost-edit/${gostId}`)} size={"small"} outline={true}>
							Редактировать
						</UrfuButton>
						<UrfuButton onClick={() => setDeleteModalOpen(true)} size={"small"} outline={true}>
							Удалить
						</UrfuButton>
						{gost.primary.status === 1 ? (
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
							Актуализировать данные
						</UrfuButton>
					</div>
				)}
				<GenericTable columns={columns} data={tableData} rowKey="id" />
			</div>
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

const DeleteCard: React.FC<DeleteCardProps> = (props) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Dialog open={isOpen} onClose={() => setIsOpen(false)}>
			<DialogTitle id="alert-dialog-title">Удалить ГОСТ?</DialogTitle>
			<DialogContent>
				<DialogContentText id="alert-dialog-description">
					Если вы удалите ГОСТ, он полностью удалится из базы без возможности восстановления
				</DialogContentText>
			</DialogContent>
			<DialogActions>
				<Button isColoredText onClick={() => setIsOpen(false)} className={styles.DeleteCardButton}>
					Отменить
				</Button>
				<Button isFilled onClick={() => onSubmitFunction()} className={styles.DeleteCardButton}>
					Удалить
				</Button>
			</DialogActions>
		</Dialog>
	);
};

interface RecoverCardProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	onSubmitFunction: () => void;
}

const RecoverCard: React.FC<RecoverCardProps> = (props) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Dialog open={isOpen} onClose={() => setIsOpen(false)}>
			<DialogTitle id="alert-dialog-title">Восстановить ГОСТ?</DialogTitle>
			<DialogContent>
				<DialogContentText id="alert-dialog-description">
					Если вы восстановите ГОСТ, он удалится из архива, а его статус будет изменён на 'Действующий'
				</DialogContentText>
			</DialogContent>
			<DialogActions>
				<Button isColoredText onClick={() => setIsOpen(false)} className={styles.DeleteCardButton}>
					Назад
				</Button>
				<Button isFilled onClick={() => onSubmitFunction()} className={styles.DeleteCardButton}>
					Восстановить
				</Button>
			</DialogActions>
		</Dialog>
	);
};

interface CancelCardProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	onSubmitFunction: () => void;
}

const CancelCard: React.FC<CancelCardProps> = (props) => {
	const { isOpen, setIsOpen, onSubmitFunction } = props;

	return (
		<Dialog open={isOpen} onClose={() => setIsOpen(false)}>
			<DialogTitle id="alert-dialog-title">Отменить ГОСТ?</DialogTitle>
			<DialogContent>
				<DialogContentText id="alert-dialog-description">
					Если вы отмените ГОСТ, он перенесется в архив, а его статус поменяться на 'Отменен'
				</DialogContentText>
			</DialogContent>
			<DialogActions>
				<Button isColoredText onClick={() => setIsOpen(false)} className={styles.DeleteCardButton}>
					Назад
				</Button>
				<Button isFilled onClick={() => onSubmitFunction()} className={styles.DeleteCardButton}>
					Отменить ГОСТ
				</Button>
			</DialogActions>
		</Dialog>
	);
};

export default GostReview;