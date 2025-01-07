import type { ReactNode } from "react";
import styles from "./GenericTable.module.scss";

interface TableColumn<T> {
    header: string;
    accessor: (row: T) => ReactNode;
}

interface GenericTableProps<T> {
    columns: TableColumn<T>[];
    data: T[];
    rowKey: keyof T;
}

export function GenericTable<T>({ columns, data, rowKey }: GenericTableProps<T>) {
    return (
        <table className={styles.table}>
            <thead>
            <tr>
                {columns.map((col) => (
                    <th key={col.header}>{col.header}</th>
                ))}
            </tr>
            </thead>
            <tbody>
            {data.map((row) => (
                <tr key={String(row[rowKey])}>
                    {columns.map((col) => (
                        <td key={col.header}>{col.accessor(row)}</td>
                    ))}
                </tr>
            ))}
            </tbody>
        </table>
    );
}