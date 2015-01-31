# encoding: utf-8

module Jekyll

  class TagPage < Page
    def initialize(site, base, dir, tag)
      @site = site
      @base = base
      @dir = dir
      @name = 'index.html'

      self.process(@name)
      self.read_yaml(File.join(base, '_layouts'), site.config['blog']['tags']['layout'])
      self.data['tag'] = tag

      tag_title_prefix = site.config['blog']['tags']['title_prefix'] || 'Тег: '
      self.data['title'] = "#{tag_title_prefix}#{tag}"
      self.data['filter_tag'] = "#{tag}"
    end
  end

  class TagPageGenerator < Generator
    safe true

    def generate(site)
      if site.layouts.key? 'tag'
        dir = site.config['blog']['tags']['url'] || 'blog/tag/'
        site.tags.keys.each do |tag|
          tag_name = tag.gsub(/\s+/, '-')
          site.pages << TagPage.new(site, site.source, File.join(dir, resolve_url(tag_name)), tag)
        end
      end
    end

    def resolve_url(raw)
      char_map = { "#"  => "sharp", "."  => "dot", " "  => "-" }
      s = ""
      raw.each_char do |c|
        if char_map.has_key?(c)
          s += char_map[c]
        else
          s += c
        end
      end
      s.downcase
    end

  end

end