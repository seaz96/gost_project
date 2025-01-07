import type { ReactNode } from "react";
import styles from "./GenericTable.module.scss";

interface GenericTableActionBlockProps {
	children?: ReactNode;
}

const GenericTableActionBlock = ({ children }: GenericTableActionBlockProps) => {
	return <div className={styles.actions}>{children}</div>;
};

export default GenericTableActionBlock;
