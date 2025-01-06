import { IconButton, Popover } from "@mui/material";
import classNames from "classnames";
import { type FC, useState } from "react";
import { Link } from "react-router-dom";
import {useAppSelector} from "../../app/hooks.ts";
import type { gostModel } from "../../entities/gost";
import eye from "../../shared/assets/eye.svg";
import pen from "../../shared/assets/pen.svg";
import styles from "./GostTable.module.scss";
import loop from "./assets/loop.png";

interface GostsTableProps {
	gosts: gostModel.GostViewInfo[];
	gostsParams: (gostModel.GostFields & { text?: string }) | null;
}

const GostsTable: React.FC<GostsTableProps> = (props) => {
	const { gosts, gostsParams } = props;


	return (
		<table className={styles.table}>
			<thead>
				<tr className={styles.tableRow}>
					<th>Код ОКС</th>
					<th>Обозначение</th>
					<th>Наименование</th>
					{gostsParams && Object.values(gostsParams).some((param) => param) && <th>Соответствие запросу</th>}
					<th>Действия</th>
				</tr>
			</thead>
			{gosts.map((gost) => (
				<GostRow key={gost.id} gostsParams={gostsParams} gost={gost} />
			))}
		</table>
	);
};

interface GostRowProps {
	gost: gostModel.GostViewInfo;
	gostsParams: (gostModel.GostFields & { text?: string }) | null;
}

const GostRow: FC<GostRowProps> = ({ gost, gostsParams }) => {
	const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
	const open = Boolean(anchorEl);
	const user = useAppSelector(s => s.user.user)

	if (gost.designation)
		return (
			<tbody>
				<IconButton
					id={gost.id.toString()}
					className={styles.loop}
					onClick={(event) => {
						setAnchorEl(event.currentTarget);
					}}
				>
					<img src={loop} alt="открыть сводку по сфере применения" />
				</IconButton>
				<Popover
					id={gost.id.toString()}
					open={open}
					onClose={() => setAnchorEl(null)}
					anchorEl={anchorEl}
					anchorOrigin={{
						vertical: "bottom",
						horizontal: "center",
					}}
					sx={{
						".css-3bmhjh-MuiPaper-root-MuiPopover-paper": {},
					}}
				>
					<div className={styles.applicationAreaContainer} style={{ whiteSpace: "pre-line" }}>
						{gost.applicationArea}
					</div>
				</Popover>
				<tr>
					<td>{gost.codeOks}</td>
					<td>{gost.designation}</td>
					<td className={styles.gostDescription}>{gost.fullName}</td>
					{gostsParams && Object.values(gostsParams).some((param) => param) && <td>{gost.relevanceMark}</td>}
					<td>
						<div className={styles.buttons}>
							<Link
								to={`/gost-review/${gost.id}`}
								className={classNames(styles.tableButton, "baseButton", "coloredText")}
							>
								<img src={eye} alt="eye" className={styles.buttonIcon} />
								Просмотр
							</Link>
							{(user?.role === "Admin" || user?.role === "Heisenberg") && (
								<Link
									to={`/gost-edit/${gost.id}`}
									className={classNames(styles.tableButton, "baseButton", "filledButton")}
								>
									<img src={pen} alt="pen" className={styles.buttonIcon} />
									Редактирование
								</Link>
							)}
						</div>
					</td>
				</tr>
			</tbody>
		);
	else return <></>;
};

export default GostsTable;