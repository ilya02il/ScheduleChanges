using Application.Common.Interfaces;
using Application.Common.Mappings.TableMappings;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Files
{
    public class WordTableFileParser<Tout> : ITableFileParser<Tout>
        where Tout : class, new()
    {
        private ITableMap<Tout> _tableMap;

        private ZipArchive _zipArchive;
        private Stream _docStream;
        private XmlReader _reader;

        public WordTableFileParser(ITableMap<Tout> tableMap)
        {
            _tableMap = tableMap;
        }

        public async Task<IEnumerable<Tout>> ParseAsync(Stream fileStream, ITableMap<Tout> tableMap = null)
        {
            return await Task.FromResult(Parse(fileStream, tableMap));
        }

        public IEnumerable<Tout> Parse(Stream fileStream, ITableMap<Tout> tableMap = null)
        {
            if (tableMap is not null)
                _tableMap = tableMap;

            _zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read);
            _docStream = _zipArchive.GetEntry("word/document.xml").Open();

            var readerSettings = new XmlReaderSettings
            {
                ValidationType = ValidationType.None
            };

            _reader = XmlReader.Create(_docStream, readerSettings);
            _reader.MoveToContent();

            int carret = 0;
            bool isFirstRow = true;
            var props = new PropertyInfo[_tableMap.ColumnMaps.Count];
            var bufModel = new Tout();

            while (_reader.Read())
            {
                if (_reader.NodeType == XmlNodeType.Element &&
                _reader.Name == "w:tc")
                {
                    if (_reader.ReadToDescendant("w:t") && _reader.Read())
                    {
                        if (isFirstRow)
                        {
                            foreach (var column in _tableMap.ColumnMaps)
                            {
                                if (column.Value.Any(v => v == _reader.Value))
                                    props[carret] = column.Key;
                            }
                            carret++;
                            continue;
                        }

                        props[carret].SetValue(bufModel, _reader.Value);
                    }
                    carret++;
                }

                if (_reader.NodeType == XmlNodeType.Element &&
                    _reader.Name == "w:tr")
                {
                    if (!isFirstRow)
                    {
                        yield return bufModel;
                        bufModel = new Tout();
                    }

                    isFirstRow = carret != _tableMap.ColumnMaps.Count;
                    carret = 0;
                }
            }
        }

        public void Dispose()
        {
            _zipArchive.Dispose();
            _docStream.Dispose();
            _reader.Dispose();
        }
    }
}
