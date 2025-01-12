import type { ReactNode } from "react";
import { Link } from "react-router-dom";
import styles from "./GenericTable.module.scss";

const GenericTableButton = ({ to, children }: { to: string; children: ReactNode }) => (
	<Link to={to} className={styles.tableButton}>
		{children}
	</Link>
);
export default GenericTableButton;
