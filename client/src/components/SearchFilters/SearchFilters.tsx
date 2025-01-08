import { useState } from "react";
import type { GostSearchParams } from "../../entities/gost/gostModel";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput";
import styles from "./SearchFilters.module.scss";

interface SearchFiltersProps {
    filters: GostSearchParams['SearchFilters'];
    onChange: (filters: GostSearchParams['SearchFilters']) => void;
}

const SearchFilters = ({ filters, onChange }: SearchFiltersProps) => {
    const [isExpanded, setIsExpanded] = useState(false);

    const handleChange = (field: keyof GostSearchParams['SearchFilters'], value: string | number | null) => {
        onChange({
            ...filters,
            [field]: value
        });
    };
    return (
        <div className={styles.filtersContainer}>
            <button
                type="button"
                className={styles.toggleButton}
                onClick={() => setIsExpanded(!isExpanded)}
            >
                {isExpanded ? 'Скрыть фильтры' : 'Показать фильтры'}
            </button>

            {isExpanded && (
                <div className={styles.filtersGrid}>
                    <UrfuTextInput
                        label="Код ОКС"
                        value={filters.CodeOks || ''}
                        onChange={(e) => handleChange('CodeOks', e.target.value || null)}
                    />
                    <UrfuTextInput
                        label="Год принятия"
                        type="number"
                        value={filters.AcceptanceYear || ''}
                        onChange={(e) => handleChange('AcceptanceYear', e.target.value ? Number(e.target.value) : null)}
                    />
                    <UrfuTextInput
                        label="Год ввода в действие"
                        type="number"
                        value={filters.CommissionYear || ''}
                        onChange={(e) => handleChange('CommissionYear', e.target.value ? Number(e.target.value) : null)}
                    />
                    <UrfuTextInput
                        label="Автор"
                        value={filters.Author || ''}
                        onChange={(e) => handleChange('Author', e.target.value || null)}
                    />
                    <UrfuTextInput
                        label="Принят впервые или взамен"
                        value={filters.AcceptedFirstTimeOrReplaced || ''}
                        onChange={(e) => handleChange('AcceptedFirstTimeOrReplaced', e.target.value || null)}
                    />
                    <UrfuTextInput
                        label="Ключевые слова"
                        value={filters.KeyWords || ''}
                        onChange={(e) => handleChange('KeyWords', e.target.value || null)}
                    />
                    <UrfuTextInput
                        label="Изменения"
                        value={filters.Changes || ''}
                        onChange={(e) => handleChange('Changes', e.target.value || null)}
                    />
                    <UrfuTextInput
                        label="Поправки"
                        value={filters.Amendments || ''}
                        onChange={(e) => handleChange('Amendments', e.target.value || null)}
                    />
                </div>
            )}
        </div>
    );
};

export default SearchFilters;