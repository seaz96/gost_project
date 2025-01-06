import {useEffect, useRef} from "react";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import styles from "./Modal.module.scss";

interface ModalProps {
	isOpen: boolean;
	setIsOpen: (isOpen: boolean) => void;
	title: string;
	description: string;
	primaryActionText: string;
	primaryAction: () => void;
	secondaryActionText: string;
}

const Modal = (props: ModalProps) => {
	const { isOpen, setIsOpen, title, description, primaryActionText, primaryAction, secondaryActionText } = props;
	const dialogRef = useRef<HTMLDialogElement>(null);

	useEffect(() => {
		if (isOpen) {
			dialogRef.current?.showModal();
		} else {
			dialogRef.current?.close();
		}
	}, [isOpen]);

	return (
		<dialog className={styles.modal} ref={dialogRef} onClose={() => setIsOpen(false)}>
			<div className={styles.modalHeader}>
				<h2>{title}</h2>
			</div>
			<div className={styles.modalContent}>
				<p>{description}</p>
			</div>
			<div className={styles.modalActions}>
				<UrfuButton size={"small"} onClick={() => setIsOpen(false)}>{secondaryActionText}</UrfuButton>
				<UrfuButton size={"small"} outline={true} onClick={() => primaryAction()}>
					{primaryActionText}
				</UrfuButton>
			</div>
		</dialog>
	);
};

export default Modal;