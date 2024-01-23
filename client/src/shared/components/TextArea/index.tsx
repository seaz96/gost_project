import classNames from 'classnames'
import React from 'react'

import styles from './TextArea.module.scss'

interface TextAreaProps {
  placeholder?: string
  className?: string
  onChange?: Function
  value?: string | undefined
 }

const TextArea: React.FC<TextAreaProps> = props => {
  const {
    placeholder,
    className,
    onChange = () => {},
    value,
  } = props

  return (
    <textarea
      placeholder={placeholder}
      className={classNames(styles.textArea, className)}
      onChange={(event) => onChange(event.target.value)}
      value={value}
    />
  )
}

export default TextArea;