using System.Collections.Generic;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class ReadStatusResponse:FiscalResponse
	{
		public ReadStatusResponse(byte[] buffer) : base(buffer)
		{
			if (!CommandPassed) return;
			var list = new List<string>();
			for (var index = 0; index < 6; index++)
			{
				list.Add(getStatusValue(data[index],index));
			}
			list.RemoveAll(x => x == string.Empty);
			Status = list.ToArray();
		}

		public string[] Status { get; set; }

		private string getStatusValue(byte statusByte,int byteIndex)
		{
			switch (byteIndex)
			{
				case 0:
				{
					if ((statusByte & 0x20) > 0)
						return "General error - this is OR of all errors marked with #";
					if ((statusByte & 0x2) > 0)
						return "# Command code is invalid.";
					if ((statusByte & 0x1) > 0)
						return "# Syntax error.";
					break;
				}
				case 1:
					if ((statusByte & 0x2) > 0)
						return "# Command is not permitted.";
					if ((statusByte & 0x1) > 0)
						return "# Overflow during command execution.";
					break;
				case 2:
					if ((statusByte & 0x20) > 0)
						return "Nonfiscal receipt is open.";
					if ((statusByte & 0x10) > 0)
						return "EJ nearly full.";
					if ((statusByte & 0x08) > 0)
						return "Fiscal receipt is open.";
					if ((statusByte & 0x04) > 0)
						return "EJ is full.";
					if ((statusByte & 0x1) > 0)
						return "# End of paper.";
					break;
				case 3:
					break;
				case 4:
					if ((statusByte & 0x20) > 0)
						return " OR of all errors marked with ‘*’ from Bytes 4 and 5.";
					if ((statusByte & 0x10) > 0)
						return "* Fiscal memory is full.";
					if ((statusByte & 0x08) > 0)
						return "There is space for less then 50 reports in Fiscal memory.";
					if ((statusByte & 0x04) > 0)
						return "FM module doesn't exist.";
					if ((statusByte & 0x02) > 0)
						return "Tax number is set.";
					if ((statusByte & 0x1) > 0)
						return "* Error while writing in FM.";
					return string.Empty;
				case 5:
					if ((statusByte & 0x10) > 0)
						return "VAT are set at least once.";
					if ((statusByte & 0x08) > 0)
						return "ECR is fiscalized.";
					if ((statusByte & 0x02) > 0)
						return "FM is formated.";
					break;
				default:
					return string.Empty;
			}
			return string.Empty;
		}
	}
}